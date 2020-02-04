import React from "react";

import 'react-toastify/dist/ReactToastify.css';
import AccountService from "../../services/AccountService";

const accountService = new AccountService();

export class ProfilePage extends React.Component {

    render() {
        return (
            <div>ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ</div>
        );
    }
}