import React from "react";

import {Link} from 'react-router-dom';

import "./LoginPage.css";

import 'react-toastify/dist/ReactToastify.css';
import AccountService from "../../services/AccountService";

const accountService = new AccountService();

export class LoginPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: '',
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(e) {
        const {name, value} = e.target;
        this.setState(
            {
                [name]: value
            });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({
            submitted: true
        });
        const {login, password} = this.state;
        if (login && password) {
            accountService.login(login, password);
        }
    }

    render() {
        const {loggingIn} = this.props;
        const {login, password, submitted} = this.state;
        return (
            <div className="col-md-6 col-md-offset-3">
                <h2>Login</h2>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !login ? ' has-error' : '')}>
                        <label htmlFor="login">Login</label>
                        <input type="text" className="form-control" name="login" value={login}
                               onChange={this.handleChange}/>
                        {submitted && !login &&
                        <div className="requiredNotify">Login is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !password ? ' has-error' : '')}>
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" name="password" value={password}
                               onChange={this.handleChange}/>
                        {submitted && !password &&
                        <div className="requiredNotify">Password is required</div>
                        }
                    </div>
                    <div className="form-group">
                        <button className="btn btn-primary">Login</button>
                        {loggingIn}
                        <Link to="/register" className="btn btn-link">Register</Link>
                    </div>
                </form>
            </div>
        );
    }
}