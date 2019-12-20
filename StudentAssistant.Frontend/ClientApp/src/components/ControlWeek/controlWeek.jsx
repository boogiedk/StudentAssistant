import moment from "moment";
import React, {Component} from 'react';
import ControlWeekService from "../../services/ControlWeekService";
import {TitleComponent} from "../TitleComponent/TitleComponent";

const title = "Расписание зачётов - Student Assistant";

const controlWeekService = new ControlWeekService();

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
                        <th>Конец</th>
                        <th>Название</th>
                    </tr>
                    </thead>
                    <tbody>
                    {controlWeekModel.controlCourseViewModel.map(courseViewModel =>
                        <tr key={courseViewModel.courseName}>
                            <td>{courseViewModel.nameOfDayWeek}</td>
                            <td>{courseViewModel.courseNumber}</td>
                            <td>{courseViewModel.startOfClasses}</td>
                            <td>{courseViewModel.endOfClasses}</td>
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
        controlWeekService.update();
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