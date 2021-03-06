import React from 'react';
import {Route} from 'react-router-dom';
import {Layout} from './components/Layout';
import {Home} from './components/Home/Home';
import {courseSchedule} from './components/CourseSchedule/courseSchedule'
import {logProvider} from './components/LogProvider/logProvider'
import {controlWeek} from "./components/ControlWeek/controlWeek";
import {examSchedule} from "./components/ExamSchedule/examSchedule";
import {ToastContainer} from "react-toastify";
import LoginPage from "./components/LoginPage/LoginPage";
import RegisterPage from "./components/RegisterPage/RegisterPage";
import {PrivateRoute} from "./components/PrivateRoute/PrivateRoute";

export default class App extends React.Component {
    static displayName = 'App.name';
    
    render() {
        
        return (
            <Layout>
                <ToastContainer/>
                <Route exact path='/' component={Home}/>
                <Route path='/courseSchedule' component={courseSchedule}/>
                <PrivateRoute path='/logs' component={logProvider}/>
                <Route path='/controlWeek' component={controlWeek}/>
                <Route path='/examSchedule' component={examSchedule}/>
                <Route path='/login' component={LoginPage}/>
                <Route path='/register' component={RegisterPage}/>
            </Layout>
        );
    }
}