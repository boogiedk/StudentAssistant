import React, {Component} from 'react';
import {Route} from 'react-router';
import {Layout} from './components/Layout';
import {Home} from './components/Home/Home';
import {courseSchedule} from './components/CourseSchedule/courseSchedule'
import {logProvider} from './components/LogProvider/logProvider'
import {controlWeek} from "./components/ControlWeek/controlWeek";
import {examSchedule} from "./components/ExamSchedule/examSchedule";
import {ToastContainer} from "react-toastify";

export default class App extends Component {
    static displayName = 'App.name';

//        <Route path='/logs' component={Logs} /> 
    render() {
        return (
            <Layout>
                <ToastContainer/>
                <Route exact path='/' component={Home}/>
                <Route path='/courseSchedule' component={courseSchedule}/>
                <Route path='/logs' component={logProvider}/>
                <Route path='/controlWeek' component={controlWeek}/>
                <Route path='/examSchedule' component={examSchedule}/>
            </Layout>
        );
    }
}
