import {createStore} from "redux";


export let initialUserState = {
    user: {}
};

function userStore(state = initialUserState, action) {

    switch (action.type) {
        case 'ADD_USER':
            return {
                user: action.user
            };
        case 'REMOVE_USER':
            return state.user = {};
        case 'GET_USER':
            return state.user;
        default:
            return state
    }
}

export function GetUser() {
    let user = store.dispatch({type: 'GET_USER'});
    return user
}

export function AddUser(user) {
    store.dispatch({type: 'ADD_USER', user: user});
}

let store = createStore(userStore);

// Create a Redux store holding the state of your app.
// Its API is { subscribe, dispatch, getState }.


// You can use subscribe() to update the UI in response to state changes.
// Normally you'd use a view binding library (e.g. React Redux) rather than subscribe() directly.
// However it can also be handy to persist the current state in the localStorage.
store.subscribe(() => console.log(store.getState()));
// The only way to mutate the internal state is to dispatch an action.
// The actions can be serialized, logged or stored and later replayed.
//store.dispatch({type: 'ADD_USER', user: {user: {login: 'fdsfs', password: 'fdsfdss'}}});