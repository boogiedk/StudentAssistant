import moment from "moment";
import React, {Component} from 'react';
import ExamScheduleService from "../../services/ExamScheduleService";
import {TitleComponent} from "../TitleComponent/TitleComponent";

import 'react-toastify/dist/ReactToastify.css';

const title = "Расписание экзаменов - Student Assistant";

const examScheduleService = new ExamScheduleService();

export class examSchedule extends Component {

    constructor(props) {
        super(props);
        this.state = {
            examScheduleModel: [],
            selectedDate: moment(new Date()).format('YYYY-MM-DD'),
            loading: true,
            groupName: 'БББО-01-16'
        };
        this.updateExamSchedule = this.updateExamSchedule.bind(this);
        this.handleChangeSelect = this.handleChangeSelect.bind(this);

        // по дефолту отправляем Date.Now()
        this.getExamSchedule();
    }

    getExamSchedule() {
        examScheduleService.get(this.state.groupName)
            .then(response => {
                this.setState(
                    {
                        examScheduleModel: response.examScheduleModel,
                        loading: response.loading
                    });
            });
    }

    static renderExamSchedule(examScheduleModel) {

        if (examScheduleService.validateExamScheduleModel(examScheduleModel)) {
            return (
                <table className='table table-striped'>
                    <thead>
                    <tr>
                        <th>Дата</th>
                        <th>Начало</th>
                        <th>Название</th>
                    </tr>
                    </thead>
                    <tbody>
                    {examScheduleModel.examCourseViewModel.map(courseViewModel =>
                        <tr key={courseViewModel.numberDate}>
                            <td>{courseViewModel.numberDate + " " + courseViewModel.dayOfWeek}</td>
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
    updateExamSchedule() {
        examScheduleService.update();
    }

    //изменение селектора групп
    handleChangeSelect(event) {
        this.setState({
            groupName: event.target.value,
        }, () => {
            this.getExamSchedule();
        });
    }

    render() {

        let contents = this.state.loading
            ? <p className="infoMessage"><em>Идет загрузка расписания...</em></p>
            : examSchedule.renderExamSchedule(this.state.examScheduleModel);

        examSchedule.getTitle();

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
                        расписания: {this.state.examScheduleModel.updateDatetime}.</i>

                    <img
                        src="https://image.flaticon.com/icons/svg/60/60961.svg"
                        title="Обновить расписание"
                        onClick={this.updateExamSchedule}
                        className="iconLoad"
                        alt="iconUpdate"/>


                    <a href="https://www.mirea.ru/upload/medialibrary/272/ekz_KBiSP_4-kurs_zima.xls">
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