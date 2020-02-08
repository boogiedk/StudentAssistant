import { createStore } from 'redux'

const initialUserState = {
    arr:[]
};

function userStore(state = initialUserState, action) {
    
    switch (action.type) {
        case 'ADD_USER':
            return {
                ...state,
                arr: [...state.arr, action.user]
            };
        case 'REMOVE_USER':
            return state.arr.splice(0,1);
        case 'GET_USER':
            return state.arr[0];
        default:
            return state
    }
}
// Create a Redux store holding the state of your app.
// Its API is { subscribe, dispatch, getState }.
let store = createStore(userStore);
// You can use subscribe() to update the UI in response to state changes.
// Normally you'd use a view binding library (e.g. React Redux) rather than subscribe() directly.
// However it can also be handy to persist the current state in the localStorage.
store.subscribe(() => console.log(store.getState()));
// The only way to mutate the internal state is to dispatch an action.
// The actions can be serialized, logged or stored and later replayed.
store.dispatch({ type: 'ADD_USER', user:{login:'fdsfs',password:'fdsfdss'} });
// 1
store.dispatch({ type: 'GET_USER' });
// 2
store.dispatch({ type: 'DECREMENT' });
// 1