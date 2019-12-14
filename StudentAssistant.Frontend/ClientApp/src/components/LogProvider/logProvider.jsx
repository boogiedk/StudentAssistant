import React, {Component} from 'react';
import {TitleComponent} from "../TitleComponent/TitleComponent";
import RestService from "../../services/RestService";

const title = "Логи - Student Assistant";

const restService = new RestService();

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
        let path = '/api/v1/log/get';

        restService.get(path)
            .then(response => {
                    this.setState(
                        {
                            logs: response,
                            loading: false
                        });
                }
            );
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
        return (
            <div>{logs.logs}</div>
        )
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
