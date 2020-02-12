import React, {Component} from 'react';
import {Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink} from 'reactstrap';
import {Link} from 'react-router-dom';
import './NavMenu.css';

import {connect} from "react-redux";
import {setAuthFlag} from "../../redux/actions/authenticationAction";
import {history} from "../../helpers/history";

 class NavMenu extends Component {
    static displayName = 'NavMenu.name';

    constructor(props) {
        super(props);
        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

     logout() {
         this.props.setIsAuth(false);
         history.push('/login');
         return {
             success: true
         };
     }


     render() {
        const LogInOutNavLink = (() => {
            const isAuthorizationUser = this.props.IsAuthentication;
            console.log(isAuthorizationUser);
            if (isAuthorizationUser) {

                return <NavLink tag={Link} className="text-dark" onClick={this.logout} to="/">Выйти</NavLink>
            }

            return <NavLink tag={Link} className="text-dark" to="/login">Войти</NavLink>
        });
        

        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">Student Assistant</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2"/>
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed}
                                  navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Главная</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/courseSchedule">Расписание
                                        пар</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/controlWeek">Расписание
                                        зачётов</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/examSchedule">Расписание
                                        экзаменов</NavLink>
                                </NavItem>
                                <NavItem>
                                    <LogInOutNavLink />
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}

const mapStateToProps = store => {
    console.log(store);
    return {
        IsAuthentication: store.authentication.IsAuthentication
    }
};

const mapDispatchToProps = dispatch => ({
    setIsAuth: flag => dispatch( setAuthFlag({
        type: 'SET_VALUE',
        payload: flag
    })),
});
 
// в наш компонент App, с помощью connect(mapStateToProps)
export default connect(mapStateToProps,mapDispatchToProps)(NavMenu);