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
using System.Diagnostics;
using System.Reflection;

namespace Xb.Simulador.Tools
{
    /// <summary>
    /// Provides loosely-coupled messaging between
    /// various colleague objects.  All references to objects
    /// are stored weakly, to prevent memory leaks.
    /// </summary>
    public class Messenger
    {
        #region Constructor
        private Messenger() { }
        #endregion // Constructor

        #region Register
        /// <summary>
        /// Registers a callback method, with no parameter, to be invoked when a specific message is broadcasted.
        /// </summary>
        /// <param name="message">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcasted.</param>
        public void Register(string message, Action callback)
        {
            this.Register(message, callback, null);
        }

        /// <summary>
        /// Registers a callback method, with a parameter, to be invoked when a specific message is broadcasted.
        /// </summary>
        /// <param name="message">The message to register for.</param>
        /// <param name="callback">The callback to be called when this message is broadcasted.</param>
        public void Register<T>(string message, Action<T> callback)
        {
            this.Register(message, callback, typeof(T));
        }

        void Register(string message, Delegate callback, Type parameterType)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("El missatge no pot estar buit.");

            if (callback == null)
                throw new ArgumentException("'Callback' buit a Messenger.");

            this.VerifyParameterType(message, parameterType);

            _messageToActionsMap.AddAction(message, callback.Target, callback.Method, parameterType);
        }

        [Conditional("DEBUG")]
        void VerifyParameterType(string message, Type parameterType)
        {
            Type previouslyRegisteredParameterType = null;
            if (_messageToActionsMap.TryGetParameterType(message, out previouslyRegisteredParameterType))
            {
                if (previouslyRegisteredParameterType != null && parameterType != null)
                {
                    if (!previouslyRegisteredParameterType.Equals(parameterType))
                        throw new InvalidOperationException(
                            string.Format(
                                "El tipus de la acció registrada es inconsistent amb la acció definida per el missatge '{0}'.\nS'esperaba {1} i s'ha rebut {2}"
                            , message
                            , previouslyRegisteredParameterType.FullName
                            , parameterType.FullName
                            )
                        );
                }
                else
                {
                    // One, or both, of previouslyRegisteredParameterType or callbackParameterType are null.
                    if (previouslyRegisteredParameterType != parameterType)   // not both null?
                    {
                        throw new TargetParameterCountException(
                            string.Format(
                                "El número de parametres de la acció registrada es inconsistent amb la acció definida per el missatge '{0}'.\nS'esperaba {1} i s'ha rebut {2}"
                            , message
                            , previouslyRegisteredParameterType == null ? 0 : 1
                            , parameterType == null ? 0 : 1
                            )
                        );
                    }
                }
            }
        }
        #endregion // Register

        #region NotifyColleagues
        /// <summary>
        /// Notifies all registered parties that a message is being broadcasted.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <param name="parameter">The parameter to pass together with the message.</param>
        public void NotifyColleagues(string message, object parameter)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("El missatge no pot estar buit.");

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(message, out registeredParameterType))
            {
                if (registeredParameterType == null)
                    throw new TargetParameterCountException(
                        string.Format(
                            "No es pot passar un paràmetre amb el missatge '{0}'. Les accions registrades no esperen cap paràmetre."
                        , message
                        )
                    );
            }

            var actions = _messageToActionsMap.GetActions(message);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke(parameter));
        }

        /// <summary>
        /// Notifies all registered parties that a message is being broadcasted.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        public void NotifyColleagues(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException(
                    "El missatge no pot estar buit."
                );

            Type registeredParameterType;
            if (_messageToActionsMap.TryGetParameterType(message, out registeredParameterType))
            {
                if (registeredParameterType != null)
                    throw new TargetParameterCountException(
                        string.Format(
                            "S'ha de passar un paràmetre del tipus {0} amb aquest missatge."
                        , registeredParameterType.FullName
                        )
                    );
            }

            var actions = _messageToActionsMap.GetActions(message);
            if (actions != null)
                actions.ForEach(action => action.DynamicInvoke());
        }

        public void Reset()
        {
            _messageToActionsMap.Reset();
        }
        #endregion // NotifyColleauges

        #region MessageToActionsMap [nested class]
        /// <summary>
        /// This class is an implementation detail of the Messenger class.
        /// </summary>
        private class MessageToActionsMap
        {
            #region Constructor
            internal MessageToActionsMap() { }
            #endregion // Constructor

            #region AddAction
            /// <summary>
            /// Adds an action to the list.
            /// </summary>
            /// <param name="message">The message to register.</param>
            /// <param name="target">The target object to invoke, or null.</param>
            /// <param name="method">The method to invoke.</param>
            /// <param name="actionType">The type of the Action delegate.</param>
            internal void AddAction(string message, object target, MethodInfo method, Type actionType)
            {
                if (message == null)
                    throw new ArgumentException("El missatge no pot estar buit.");

                if (method == null)
                    throw new ArgumentException("El mètod no pot estar buit.");

                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                        _map[message] = new List<WeakAction>();

                    _map[message].Add(new WeakAction(target, method, actionType));
                }
            }
            #endregion // AddAction

            #region Reset
            internal void Reset()
            {
                _map.Clear();
            }
            #endregion

            #region GetActions
            /// <summary>
            /// Gets the list of actions to be invoked for the specified message
            /// </summary>
            /// <param name="message">The message to get the actions for</param>
            /// <returns>Returns a list of actions that are registered to the specified message</returns>
            internal List<Delegate> GetActions(string message)
            {
                if (message == null)
                    throw new ArgumentException("El missatge no pot estar buit.");

                List<Delegate> actions;
                lock (_map)
                {
                    if (!_map.ContainsKey(message))
                        return null;

                    List<WeakAction> weakActions = _map[message];
                    actions = new List<Delegate>(weakActions.Count);
                    for (int i = weakActions.Count - 1; i > -1; --i)
                    {
                        WeakAction weakAction = weakActions[i];
                        if (weakAction == null)
                            continue;

                        Delegate action = weakAction.CreateAction();
                        if (action != null)
                        {
                            actions.Add(action);
                        }
                        else
                        {
                            // The target object is dead, so get rid of the weak action.
                            weakActions.Remove(weakAction);
                        }
                    }

                    // Delete the list from the map if it is now empty.
                    if (weakActions.Count == 0)
                        _map.Remove(message);
                }

                // Reverse the list to ensure the callbacks are invoked in the order they were registered.
                actions.Reverse();

                return actions;
            }
            #endregion // GetActions

            #region TryGetParameterType
            /// <summary>
            /// Get the parameter type of the actions registered for the specified message.
            /// </summary>
            /// <param name="message">The message to check for actions.</param>
            /// <param name="parameterType">
            /// When this method returns, contains the type for parameters 
            /// for the registered actions associated with the specified message, if any; otherwise, null.
            /// This will also be null if the registered actions have no parameters.
            /// This parameter is passed uninitialized.
            /// </param>
            /// <returns>true if any actions were registered for the message</returns>
            internal bool TryGetParameterType(string message, out Type parameterType)
            {
                if (message == null)
                    throw new ArgumentException("El missatge no pot estar buit.");

                parameterType = null;
                List<WeakAction> weakActions;
                lock (_map)
                {
                    if (!_map.TryGetValue(message, out weakActions) || weakActions.Count == 0)
                        return false;
                }
                parameterType = weakActions[0].ParameterType;
                return true;
            }
            #endregion // TryGetParameterType

            #region Fields
            // Stores a hash where the key is the message and the value is the list of callbacks to invoke.
            readonly Dictionary<string, List<WeakAction>> _map = new Dictionary<string, List<WeakAction>>();
            #endregion // Fields
        }
        #endregion // MessageToActionsMap [nested class]

        #region WeakAction [nested class]
        /// <summary>
        /// This class is an implementation detail of the MessageToActionsMap class.
        /// </summary>
        private class WeakAction
        {
            #region Constructor
            /// <summary>
            /// Constructs a WeakAction.
            /// </summary>
            /// <param name="target">The object on which the target method is invoked, or null if the method is static.</param>
            /// <param name="method">The MethodInfo used to create the Action.</param>
            /// <param name="parameterType">The type of parameter to be passed to the action. Pass null if there is no parameter.</param>
            internal WeakAction(object target, MethodInfo method, Type parameterType)
            {
                if (target == null)
                {
                    _targetRef = null;
                }
                else
                {
                    _targetRef = new WeakReference(target);
                }

                _method = method;

                this.ParameterType = parameterType;

                if (parameterType == null)
                {
                    _delegateType = typeof(Action);
                }
                else
                {
                    _delegateType = typeof(Action<>).MakeGenericType(parameterType);
                }
            }
            #endregion // Constructor

            #region CreateAction
            /// <summary>
            /// Creates a "throw away" delegate to invoke the method on the target, or null if the target object is dead.
            /// </summary>
            internal Delegate CreateAction()
            {
                // Rehydrate into a real Action object, so that the method can be invoked.
                if (_targetRef == null)
                {
                    return Delegate.CreateDelegate(_delegateType, _method);
                }
                else
                {
                    try
                    {
                        object target = _targetRef.Target;
                        if (target != null)
                            return Delegate.CreateDelegate(_delegateType, target, _method);
                    }
                    catch
                    {
                    }
                }

                return null;
            }
            #endregion // CreateAction

            #region Fields
            internal readonly Type ParameterType;
            readonly Type _delegateType;
            readonly MethodInfo _method;
            readonly WeakReference _targetRef;
            #endregion // Fields
        }

        #endregion // WeakAction [nested class]

        #region Fields
        readonly MessageToActionsMap _messageToActionsMap = new MessageToActionsMap();

        static public Messenger Instance
        {
            get { return _instance; }
        }
        static private Messenger _instance = new Messenger();

        #endregion // Fields
    }
}