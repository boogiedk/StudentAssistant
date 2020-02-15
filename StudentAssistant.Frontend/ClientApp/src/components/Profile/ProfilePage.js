import React from "react";
import {Link} from "react-router-dom";
import AccountService from "../../services/AccountService";
const accountService = new AccountService();

export default class ProfilePage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {
                firstName: '',
                lastName: '',
                login: '',
                groupName: '',
            }
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        
        this.getProfile();
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
    }

    getProfile()
    {
        accountService.getProfile().then(result => {
            if (result.success) {
                console.log(result);
                this.setState({
                    user: result.user
                })
            }
        });
    }
    
    logout()
    {
        console.log('here');
        accountService.logout();
    }



    render() {
        const {user} = this.state;
        return (
            <div>
                <a>{user.firstName}</a>
                <a>{user.lastName}</a>
                <a>{user.groupName}</a>
                <a>{user.login}</a>
                <button onClick={this.logout}>Logout</button>
            </div>
        );
    }
}