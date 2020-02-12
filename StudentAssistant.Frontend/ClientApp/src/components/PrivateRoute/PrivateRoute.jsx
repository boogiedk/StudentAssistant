import React, {Component} from 'react';
import {Redirect, Route} from 'react-router-dom';
import {connect} from "react-redux";


class PrivateRoute extends React.Component {
    constructor(props) {
        super(props);
    }
    
    render()
    {
        const PrivateRoute = ({component: Component, roles, ...rest}) => (
            <Route {...rest} render={props => {
                const authorizationToken = this.props.IsAuthentication;
                if (!authorizationToken) {
                    return <Redirect to={{pathname: '/login', state: {from: props.location}}}/>
                }

                //  if (roles && roles.indexOf(currentUser.role) === -1) {
                //       return <Redirect to={{ pathname: '/'}} />
                //   }

                return <Component {...props} />
            }}/>
        );

        return(<PrivateRoute/>)
    }
}

const mapStateToProps = store => {
    console.log(store);
    return {
        IsAuthentication: store.authentication.IsAuthentication
    }
};

export default connect(mapStateToProps)(PrivateRoute);
