import React from 'react';
import { Link } from 'react-router-dom';
import AccountService from "../../services/AccountService";

const accountService = new AccountService();

export class RegisterPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {
                firstName: '',
                lastName: '',
                login: '', 
                groupName:'',
                password: ''
            },
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const { name, value } = event.target;
        const { user } = this.state;
        this.setState({
            user: {
                ...user,
                [name]: value
            }
        });
    }

    handleSubmit(event) {
        event.preventDefault();

        this.setState({ submitted: true });
        const { user } = this.state;
        if (user.firstName && user.lastName && user.login && user.password && user.groupName) {
            accountService.register(user);
        }
    }

    render() {
        const { registering  } = this.props;
        const { user, submitted } = this.state;
        return (
            <div className="col-md-6 col-md-offset-3">
                <h2>Register</h2>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !user.firstName ? ' has-error' : '')}>
                        <label htmlFor="firstName">First Name</label>
                        <input type="text" className="form-control" name="firstName" value={user.firstName} onChange={this.handleChange} />
                        {submitted && !user.firstName &&
                        <div className="requiredNotify">First Name is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.lastName ? ' has-error' : '')}>
                        <label htmlFor="lastName">Last Name</label>
                        <input type="text" className="form-control" name="lastName" value={user.lastName} onChange={this.handleChange} />
                        {submitted && !user.lastName &&
                        <div className="requiredNotify">Last Name is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.groupName ? ' has-error' : '')}>
                        <label htmlFor="groupName">Group name</label>
                        <input type="text" className="form-control" name="groupName" value={user.groupName} onChange={this.handleChange} />
                        {submitted && !user.groupName &&
                        <div className="requiredNotify">Group name is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.login ? ' has-error' : '')}>
                        <label htmlFor="login">Username</label>
                        <input type="text" className="form-control" name="login" value={user.login} onChange={this.handleChange} />
                        {submitted && !user.login &&
                        <div className="requiredNotify">Login is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.password ? ' has-error' : '')}>
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" name="password" value={user.password} onChange={this.handleChange} />
                        {submitted && !user.password &&
                        <div className="requiredNotify">Password is required</div>
                        } 
                    </div>
                    <div className="form-group">
                        <button className="btn btn-primary">Register</button>
                        {registering}
                        <Link to="/login" className="btn btn-link">Cancel</Link>
                    </div>
                </form>
            </div>
        );
    }
}