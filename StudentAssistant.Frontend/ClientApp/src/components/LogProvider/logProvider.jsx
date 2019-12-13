import React, {Component} from 'react';
import {TitleComponent} from "../TitleComponent/TitleComponent";

const title = "Логи - Student Assistant";

const url = 'http://localhost:18935';

export class logProvider extends Component {

    constructor(props) {
        super(props);
        this.state = {
            logs: [],
        };

        this.getLogs();
    }

    static displayName = logProvider.name;

    getLogs() {
        let path = url + '/api/v1/log/get';

        fetch(path, {
            method: 'GET',
            headers: {
                'Accept': ' application/json',
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
            }
        })
            .then(res => res.json())
            .then(data => {
                this.setState({logs: data});
            })
            .catch(error => console.error('Error:', error));
    }

    /*костыль*/
    static getTitle() {
        return (
            <React.Fragment>
                <TitleComponent title={title}/>
            </React.Fragment>
        );
    }

    static renderCourseSchedule(logs) {
        {
          return (<a> {logs.logs} </a>)
        }
    }

    render() {

        let contents = logProvider.renderCourseSchedule(this.state.logs);

        return (
            <div>
                {logProvider.getTitle()}
                <h1>Логи за сегодняшний день.</h1>
                {contents}
            </div>
        );
    }
}
