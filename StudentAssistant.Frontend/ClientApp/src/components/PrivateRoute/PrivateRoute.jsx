import React, {Component} from 'react';
import {Redirect, Route} from 'react-router-dom';
import Cookies from 'js-cookie';
import AccountService from "../../services/AccountService";

const accountService = new AccountService();

export const PrivateRoute = ({component: Component, ...rest}) => (
    <Route {...rest} render={ props => {
        accountService.isAuth();
        let cookieValue = Cookies.get('isAuth');
        let isAuth = cookieValue === 'true';
        if (isAuth) {
            return <Component {...props} />
        } else {
            return <Redirect to={{pathname: '/login', state: {from: props.location}}}/>
        }
    }}/>);