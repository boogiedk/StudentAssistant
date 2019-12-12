import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home/Home';
import {courseSchedule} from './components/CourseSchedule/courseSchedule'
import {Logs} from './components/Logs/Logs'

export default class App extends Component {
  static displayName = 'App.name';
//        <Route path='/logs' component={Logs} /> 
  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/courseSchedule' component={courseSchedule} />
          <Route path='/logs' component={Logs} />
      </Layout>
    );
  }
}
