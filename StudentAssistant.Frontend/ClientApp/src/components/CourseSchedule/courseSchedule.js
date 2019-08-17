import React, { Component } from 'react';
import "./courseSchedule.css";

const url = 'http://localhost:18936';

export class courseSchedule extends Component {

    constructor(props) {
        super(props);
        this.state = {
            courseScheduleModel: [],
            dateTimeString: '',
            selectedDate: new Date().getFullYear() + '-' + new Date().getMonth() + '-' + new Date().getDate(), //toLocaleDateString(),
            loading: true,

        };
        this.handleChange = this.handleChange.bind(this);    
        this.selectedDate = new Date();
        // по дефолту отправляем Date.Now()
        this.getCourseScheduleModel(this.selectedDate);
    }

    handleChange(event) {
        this.setState({
            selectedDate: event.target.value,
        });
        this.getCourseScheduleModel(event.target.value);
    }

    getCourseScheduleModel(selectedDatetime) {
        let path = url + '/api/v1/schedule/selected';

        let requestModel = {
            dateTimeRequest: selectedDatetime // '2019-03-13T14:15:56.278Z'
        };

        console.log(JSON.stringify(requestModel));

        fetch(path, {
            method: 'POST',
            headers: {
                'Accept': ' application/json',
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
            },
            body: JSON.stringify(requestModel)
        })
            .then(res => res.json())
            .then(data => {
                this.setState({ courseScheduleModel: data, loading: false });
            })
            .then(response => JSON.stringify(response))
            .catch(error => console.error('Error:', error));
    }

    static renderCourseSchedule(courseScheduleModel) {
        return (
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>№</th>
                        <th>Название</th>
                        <th>Кабинет</th>
                        <th>Тип</th>
                        <th>Преподаватель</th>
                        <th>Номера</th>
                        <th>Четность</th>
                    </tr>
                </thead>
                <tbody>
                    {courseScheduleModel.coursesViewModel.map(courseViewModel =>
                        <tr key={courseViewModel.courseNumber}>
                            <td>{courseViewModel.courseNumber}</td>
                            <td>{courseViewModel.courseName}</td>
                            <td>{courseViewModel.coursePlace}</td>
                            <td>{courseViewModel.courseType}</td>
                            <td>{courseViewModel.teacherFullName}</td>
                            <td>{courseViewModel.numberWeek}</td>
                            <td>{courseViewModel.parityWeek}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );

    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : courseSchedule.renderCourseSchedule(
                this.state.courseScheduleModel
            );

        return (
            <div>
                <h1>Расписание</h1>

                <p>На странице отображено расписание на {this.state.courseScheduleModel.datetimeRequest}, <b>{this.state.courseScheduleModel.nameOfDayWeek}</b></p>

                <p>
                    <label className="labelChooseDate">Выберите дату: </label>
                    <input
                        type="date"
                        className="inputTextbox"
                        value={this.state.selectedDate}
                        onChange={this.handleChange}
                        required="required"
                        name="inputTextbox" 
                        id="inputTextbox"
                    />
                </p>

                {contents}

                <i>Последнее обновление расписания: {this.state.courseScheduleModel.updateDatetime}</i>
            </div>
        );
    }

}
