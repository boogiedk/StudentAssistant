const initialState = {
    IsAuthentication: false,
};

export function authenticationReducer(state = initialState, action) {
    console.log(action);
    switch (action.type) {
        case 'SET_VALUE':
            return {...state, IsAuthentication: action.payload};

        default:
            return state
    }
    
}