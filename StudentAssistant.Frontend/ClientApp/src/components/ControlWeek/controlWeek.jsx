import moment from "moment";
import React, {Component} from 'react';
import ControlWeekService from "../../services/ControlWeekService";
import {TitleComponent} from "../TitleComponent/TitleComponent";

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import ToastNotificationService from "../../services/ToastNotificationService";

const title = "Расписание зачётов - Student Assistant";

const controlWeekService = new ControlWeekService();
const toastNotificationService = new ToastNotificationService();

export class controlWeek extends Component {
    
    constructor(props) {
        super(props);
        this.state = {
            controlWeekModel: [],
            selectedDate: moment(new Date()).format('YYYY-MM-DD'),
            loading: true,
            groupName: 'БББО-01-16'
        };
        this.updateControlWeek= this.updateControlWeek.bind(this);
        this.handleChangeSelect = this.handleChangeSelect.bind(this);

        // по дефолту отправляем Date.Now()
        this.getControlWeek();
    }
    
    getControlWeek()
    {
        controlWeekService.get(this.state.groupName)
            .then(response => {
                this.setState(
                    {
                        controlWeekModel: response,
                        loading: false
                    });
            });
    }
    
    static renderControlWeek(controlWeekModel) {

        if (controlWeekService.validate(controlWeekModel)) {
            return (
                <table className='table table-striped'>
                    <thead>
                    <tr>
                        <th>День недели</th>
                        <th>№</th>
                        <th>Начало</th>
                        <th>Название</th>
                    </tr>
                    </thead>
                    <tbody>
                    {controlWeekModel.controlCourseViewModel.map(courseViewModel =>
                        <tr key={courseViewModel.courseName}>
                            <td>{controlWeekService.prepareNameOfDayWeek(courseViewModel.nameOfDayWeek)}</td>
                            <td>{courseViewModel.courseNumber}</td>
                            <td>{courseViewModel.startOfClasses}</td>
                            <td>
                                <div className="courseNameStyle"> {courseViewModel.courseName} </div>
                                <div className="coursePlaceStyle"> Аудитория {courseViewModel.coursePlace}</div>
                                <div className="courseTypeStyle"> {courseViewModel.courseType}</div>
                                <div className="teacherFullNameStyle"> {courseViewModel.teacherFullName}</div>
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            );
        } else {
            return (
                <p className="infoMessage"><em>Идет загрузка расписания...</em>
                </p>
            );
        }
    }

    static getTitle() {
        return (
            <React.Fragment>
                <TitleComponent title={title}/>
            </React.Fragment>
        );
    }

    // обновить расписание (скачать новый файл на сервер)
    updateControlWeek() {
        controlWeekService.update()
            .then(response => toastNotificationService.notifyInfo(response));
    }

    //изменение селектора групп
    handleChangeSelect(event) {
        this.setState({
            groupName: event.target.value,
        }, () => {
            this.getControlWeek();
        });
    }
    
    render() {

        let contents = this.state.loading
            ? <p className="infoMessage"><em>Идет загрузка расписания...</em></p>
            : controlWeek.renderControlWeek(this.state.controlWeekModel);

        controlWeek.getTitle();

        if (contents != null) {
            return (
                <div>
                    <React.Fragment>
                        <TitleComponent title={title}/>
                    </React.Fragment>

                    <h1>Расписание</h1>

                    <p>
                        <label className="labelChooseGroup">Выберите группу: </label>
                        <select name="GroupNames" value={this.state.groupName} onChange={this.handleChangeSelect}>
                            <option value="БББО-01-16">БББО-01-16</option>
                            <option value="БББО-02-16">БББО-02-16</option>
                            <option value="БББО-03-16">БББО-03-16</option>
                        </select>
                    </p>
                    
                    {contents}

                    <i className="bottom">Последнее обновление
                        расписания: {this.state.controlWeekModel.updateDatetime}.</i>

                    <img
                        src="https://image.flaticon.com/icons/svg/60/60961.svg"
                        title="Обновить расписание"
                        onClick={this.updateControlWeek}
                        className="iconLoad"
                        alt="iconUpdate"/>


                    <a href="https://www.mirea.ru/upload/medialibrary/28e/zach_KBiSP_4-kurs_zima.xlsx">
                        <img
                            src="https://image.flaticon.com/icons/svg/152/152555.svg"
                            title="Скачать расписание"
                            className="iconDownload"
                            alt="Download"
                        /></a>
                    
                </div>
            );
        } else {
            return (
                <p className="infoMessage"><em>Идет загрузка расписания...</em>
                </p>
            );
        }

    }
    
}