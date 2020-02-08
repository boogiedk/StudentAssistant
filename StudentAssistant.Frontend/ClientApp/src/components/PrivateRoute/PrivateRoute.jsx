import React from 'react';
import {Redirect, Route} from 'react-router-dom';
import {GetUser} from "../../services/StoreService";

export const PrivateRoute = ({component: Component, roles, ...rest}) => (
    <Route {...rest} render={props => {
        let user = GetUser();
        console.log(user);
        const authorizationToken = user;
        if (!authorizationToken) {
            return <Redirect to={{pathname: '/login', state: {from: props.location}}}/>
        }

        //  if (roles && roles.indexOf(currentUser.role) === -1) {
        //       return <Redirect to={{ pathname: '/'}} />
        //   }
        
        return <Component {...props} />
    }}/>
);
