import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home/Home';
import {courseSchedule} from './components/CourseSchedule/courseSchedule'
import {logProvider} from './components/LogProvider/logProvider'
import {controlWeek} from "./components/ControlWeek/controlWeek";

export default class App extends Component {
  static displayName = 'App.name';
//        <Route path='/logs' component={Logs} /> 
  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/courseSchedule' component={courseSchedule} />
          <Route path='/logs' component={logProvider} />
          <Route path='/controlWeek' component={controlWeek} />
      </Layout>
    );
  }
}
