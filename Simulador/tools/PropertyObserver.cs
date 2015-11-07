/*
 * Copyright (c) 2011 Xavier Borrell i Gómez
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

namespace Xb.Simulador.Tools
{
    /// <summary>
    /// Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
    /// and executes callback methods (i.e. handlers) registered for properties of that object.
    /// </summary>
    /// <typeparam name="TPropertySource">The type of object to monitor for property changes.</typeparam>
    public class PropertyObserver<TPropertySource> : IWeakEventListener
            where TPropertySource : INotifyPropertyChanged
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of PropertyObserver, which
        /// observes the 'propertySource' object for property changes.
        /// </summary>
        /// <param name="propertySource">The object to monitor for property changes.</param>
        public PropertyObserver(TPropertySource propertySource)
        {
            if (propertySource == null)
                throw new ArgumentNullException("Propietat invàlida al observador");

            _propertySourceRef = new WeakReference(propertySource);
            _propertyNameToHandlerMap = new Dictionary<string, Action<TPropertySource>>();
        }
        #endregion // Constructor

        #region Public Methods
        #region RegisterHandler
        /// <summary>
        /// Registers a callback to be invoked when the PropertyChanged event has been raised for the specified property.
        /// </summary>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="handler">The callback to invoke when the property has changed.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(
                Expression<Func<TPropertySource, object>> expression,
                Action<TPropertySource> handler)
        {
            if (expression == null)
                throw new ArgumentNullException("Expressió buida al observador");

            string propertyName = GetPropertyName(expression);
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("L'expressió no ofereix un nom de propietat.");

            if (handler == null)
                throw new ArgumentException("Manegador buit al observador");

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                Debug.Assert(
                    !_propertyNameToHandlerMap.ContainsKey(propertyName)
                , string.Format("Perqué es tracta de tornar a registrar la propietat {0} un altre cop?", propertyName)
                );

                _propertyNameToHandlerMap[propertyName] = handler;
                PropertyChangedEventManager.AddListener(propertySource, this, propertyName);
            }

            return this;
        }

        #endregion // RegisterHandler

        #region UnregisterHandler
        /// <summary>
        /// Removes the callback associated with the specified property.
        /// </summary>
        /// <param name="propertyName">A lambda expression like 'n => n.PropertyName'.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("Expressió buida al observador");

            string propertyName = GetPropertyName(expression);
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("L'expressió no ofereix un nom de propietat.");

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                if (_propertyNameToHandlerMap.ContainsKey(propertyName))
                {
                    _propertyNameToHandlerMap.Remove(propertyName);
                    PropertyChangedEventManager.RemoveListener(propertySource, this, propertyName);
                }
            }
            return this;
        }
        #endregion // UnregisterHandler
        #endregion // Public Methods

        #region IWeakEventListener Members
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            bool handled = false;

            if (managerType == typeof(PropertyChangedEventManager))
            {
                PropertyChangedEventArgs args = e as PropertyChangedEventArgs;
                if (args != null && sender is TPropertySource)
                {
                    string propertyName = args.PropertyName;
                    TPropertySource propertySource = (TPropertySource)sender;

                    if (String.IsNullOrEmpty(propertyName))
                    {
                        // When the property name is empty, all properties are considered to be invalidated.
                        // Iterate over a copy of the list of handlers, in case a handler is registered by a callback.
                        foreach (Action<TPropertySource> handler in _propertyNameToHandlerMap.Values.ToArray())
                            handler(propertySource);

                        handled = true;
                    }
                    else
                    {
                        Action<TPropertySource> handler;
                        if (_propertyNameToHandlerMap.TryGetValue(propertyName, out handler))
                        {
                            handler(propertySource);

                            handled = true;
                        }
                    }
                }
            }

            return handled;
        }
        #endregion // IWeakEventListener Members

        #region Private Helpers
        #region GetPropertyName
        static string GetPropertyName(Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(
                memberExpression != null
            , "Si us plau indiqueu una expressió lambda com a 'n => n.PropertyName'"
            );

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }
        #endregion // GetPropertyName

        #region GetPropertySource
        TPropertySource GetPropertySource()
        {
            try
            {
                return (TPropertySource)_propertySourceRef.Target;
            }
            catch
            {
                return default(TPropertySource);
            }
        }
        #endregion // GetPropertySource
        #endregion // Private Helpers

        #region Fields
        readonly Dictionary<string, Action<TPropertySource>> _propertyNameToHandlerMap;
        readonly WeakReference _propertySourceRef;
        #endregion // Fields
    }
}