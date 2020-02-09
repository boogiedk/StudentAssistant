import React from 'react';
import {Redirect, Route} from 'react-router-dom';

export const PrivateRoute = ({component: Component, roles, ...rest}) => (
    <Route {...rest} render={props => {
        const authorizationToken = true;
        if (!authorizationToken) {
            return <Redirect to={{pathname: '/login', state: {from: props.location}}}/>
        }

        //  if (roles && roles.indexOf(currentUser.role) === -1) {
        //       return <Redirect to={{ pathname: '/'}} />
        //   }
        
        return <Component {...props} />
    }}/>
);
