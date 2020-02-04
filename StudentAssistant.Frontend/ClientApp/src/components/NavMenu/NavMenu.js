import React, {Component} from 'react';
import {Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink} from 'reactstrap';
import {Link, Route} from 'react-router-dom';
import './NavMenu.css';

import { history } from '../../helpers/history';

export const PrivateNavLink = ({component: Component, roles, ...rest}) => (
    <Route {...rest} render={props => {
        const currentUser = localStorage.getItem("token");
        if (currentUser) {
            // not logged in so redirect to login page with the return url
            return <NavLink tag={Link} className="text-dark" to="/logout">Выйти</NavLink>
        }

        return <NavLink tag={Link} className="text-dark" to="/login">Войти</NavLink>

    }}/>
);

export class NavMenu extends Component {
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

    isLogout() {
        console.log("click");
        localStorage.removeItem("token");
        history.push('/login');
    }
    

    render() {
        const PrivateNavLink2 = (() => {

            const currentUser = localStorage.getItem("token");
            if (currentUser) {

                return <NavLink tag={Link} className="text-dark" onClick={this.isLogout} to="/">Выйти</NavLink>
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
                                    <PrivateNavLink2/>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
