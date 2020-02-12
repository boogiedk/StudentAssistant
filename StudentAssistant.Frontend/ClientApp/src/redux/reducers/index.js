import {authenticationReducer} from "./authenticationReducer";
import {userReducer} from "./userReducer";
import {combineReducers} from "redux";

export const rootReducer = combineReducers({
    authentication: authenticationReducer,
    user: userReducer,
});