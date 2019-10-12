import React, {Component} from 'react';
import "./courseSchedule.css";


import Popup from "reactjs-popup";

import DatePicker, {registerLocale} from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import 'react-datepicker/dist/react-datepicker-cssmodules.css';
import ru from "date-fns/locale/ru";

import 'bootstrap/dist/css/bootstrap.min.css';

import moment from 'moment';
import {TitleComponent} from "../TitleComponent/TitleComponent";

registerLocale("ru", ru);

const url = 'http://localhost:18936';

const title = "Расписание - Student Assistant";

export class courseSchedule extends Component {
    counter = 0;

    constructor(props) {
        super(props);
        this.state = {
            courseScheduleModel: [],
            dateTimeString: '',
            selectedDate: moment(new Date()).format('YYYY-MM-DD'),
            loading: true,
            groupName: 'БББО-01-16',
            isWeekday: true,
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeCalendar = this.handleChangeCalendar.bind(this);
        this.handleChangeSelect = this.handleChangeSelect.bind(this);
        this.handleChangeNextDay = this.handleChangeNextDay.bind(this);
        this.handleChangeLastDay = this.handleChangeLastDay.bind(this);


        // default values
        this.groupName = 'БББО-01-16';
        this.selectedDate = moment(new Date()).format('YYYY-MM-DD');

        // по дефолту отправляем Date.Now()
        this.getCourseScheduleModel(this.selectedDate);
    }

    handleChange(event) {
        this.setState({
            selectedDate: event.target.value,
        });

        this.getCourseScheduleModel(event.target.value);
    }

    handleChangeNextDay() {

        this.counter = this.counter + 1;

        var myDate = new Date();
        myDate.setDate(myDate.getDate() + this.counter);

        this.setState({
            selectedDate: moment(myDate).format('YYYY-MM-DD'),
        });

        this.getCourseScheduleModel(myDate);
    }

    handleChangeLastDay() {

        this.counter = this.counter - 1;

        var myDate = new Date();
        myDate.setDate(myDate.getDate() + this.counter);

        this.setState({
            selectedDate: moment(myDate).format('YYYY-MM-DD'),
        });

        this.getCourseScheduleModel(myDate);
    }

    handleChangeCalendar(date) {
        this.setState({
            selectedDate: moment(date).format('YYYY-MM-DD')
        });

        this.getCourseScheduleModel(date);
    }

    handleChangeSelect(event) {
        this.setState({
            groupName: event.target.value,
        });
        
        this.getCourseScheduleModelByGroup(event.target.value);
    }

    isWeekday(date) {
        const day = date.getDay();
        return day !== 0 //&& day !== 6 // воскресение заблокировано
    }

    getCourseScheduleModel(selectedDatetime) {
        let path = url + '/api/v1/schedule/selected';
        
        let requestModel = {
            dateTimeRequest: selectedDatetime,
            groupName: this.state.groupName
        };

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
                this.setState({courseScheduleModel: data, loading: false});
            })
            .then(response => JSON.stringify(response))
            .catch(error => console.error('Error:', error));
    }

    getCourseScheduleModelByGroup(groupName) {
        let path = url + '/api/v1/schedule/selected';

        let requestModel = {
            dateTimeRequest: this.selectedDate,
            groupName: groupName
        };

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
                this.setState({courseScheduleModel: data, loading: false});
            })
            .then(response => JSON.stringify(response))
            .catch(error => console.error('Error:', error));
    }

    static renderCourseSchedule(courseScheduleModel) {

        if ((typeof courseScheduleModel == "undefined") || (
            courseScheduleModel.coursesViewModel.length === 1 &
            courseScheduleModel.coursesViewModel[0].courseName === "")) {
            return (
                // если кол-во моделей 1 - значит есть вероятность, что модель пустая
                // далее проверяем имя первой пары и убеждаемся в этом
                <p className="infoMessage"><em>Пар не найдено. Попробуйте выбрать другой день</em></p>)
        }

        return (
            <table className='table table-striped'>
                <thead>
                <tr>
                    <th>№</th>
                    <th>Начало</th>
                    <th>Конец</th>
                    <th>Название</th>
                </tr>
                </thead>
                <tbody>
                {courseScheduleModel.coursesViewModel.map(courseViewModel =>
                    <tr key={courseViewModel.courseNumber}>
                        <td>{courseViewModel.courseNumber}</td>
                        <td>{courseViewModel.startOfClasses}</td>
                        <td>{courseViewModel.endOfClasses}</td>
                        <td>
                            <div className="courseNameStyle"> {courseViewModel.courseName} </div>
                            <div className="coursePlaceStyle"> Аудитория {courseViewModel.coursePlace}</div>
                            <div className="courseTypeStyle"> {courseViewModel.courseType}</div>
                            <div className="teacherFullNameStyle"> {courseViewModel.teacherFullName}</div>

                            <Popup trigger={<button className="icon"></button>} position="top center">
                                <div>
                                    <div
                                        className="combinedGroupStyle"> {courseViewModel.combinedGroup !== "" ? "Совмещено с " + courseViewModel.combinedGroup : "Несовмещенная пара"}</div>
                                    Пара повторяется на <div
                                    className="numberWeekStyle"> {courseViewModel.numberWeek !== "" ? "" + courseViewModel.numberWeek : "каждой " + courseViewModel.parityWeek}</div> неделе.
                                </div>
                            </Popup>
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
        );

    }

    static getTitle() {
        return (
            <React.Fragment>
                <TitleComponent title={title}/>
            </React.Fragment>
        );
    }

    render() {

        let contents = this.state.loading
            ? <p className="infoMessage"><em>Идет загрузка расписания...</em></p>
            : courseSchedule.renderCourseSchedule(this.state.courseScheduleModel);

        courseSchedule.getTitle();

        if (contents != null) {
            return (
                <div>
                    <React.Fragment>
                        <TitleComponent title={title}/>
                    </React.Fragment>

                    <h1>Расписание</h1>
                    {
                        this.state.courseScheduleModel.length !== 0 ?
                            <p>На странице отображено расписание на {this.state.courseScheduleModel.datetimeRequest},
                                <b> {this.state.courseScheduleModel.nameOfDayWeek}</b>, {this.state.courseScheduleModel.numberWeek === 3
                                    ? this.state.courseScheduleModel.numberWeek + "-я"
                                    : this.state.courseScheduleModel.numberWeek + "-ая "} неделя.
                            </p> : <p className="infoMessage"><em>Идет загрузка данных...</em></p>}
                    <p>
                        <label className="labelChooseGroup">Выберите группу: </label>
                        <select name="GroupNames" value={this.state.groupName} onChange={this.handleChangeSelect}>
                            <option value="БББО-01-16">БББО-01-16</option>
                            <option value="БББО-02-16">БББО-02-16</option>
                            <option value="БББО-03-16">БББО-03-16</option>
                        </select>
                    </p>

                    <span>
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
                    
                    <img
                        src="https://image.flaticon.com/icons/svg/271/271220.svg"
                        onClick={this.handleChangeLastDay}
                        className="leftArrow"
                        alt="left"
                    />
                     
                    <DatePicker
                        className="calendarIcon"
                        value={this.state.selectedDate}
                        onChange={this.handleChangeCalendar}
                        locale="ru"
                        customInput={
                            <button className="btn" id="calendarIcon"/>
                        }
                        disabledKeyboardNavigation
                        dateFormat="YYYY-MM-DD"
                        popperPlacement="bottom-end"
                        filterDate={this.isWeekday}
                    />
                    
                       <img
                           src="https://image.flaticon.com/icons/svg/271/271228.svg"
                           onClick={this.handleChangeNextDay}
                           className="rightArrow"
                        alt="right"/> 
                        
                </span>

                    {contents}

                    <i>Последнее обновление расписания: {this.state.courseScheduleModel.updateDatetime}.</i>
                </div>
            );
        } else {
            return (
                <p className="infoMessage"><em>Идет загрузка расписания...</em></p>
            );
        }

    }
}