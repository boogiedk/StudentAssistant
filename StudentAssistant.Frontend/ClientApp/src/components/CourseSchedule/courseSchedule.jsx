import React from 'react';
import "./courseSchedule.css";
import Popup from "reactjs-popup";
import DatePicker, {registerLocale} from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import 'react-datepicker/dist/react-datepicker-cssmodules.css';
import 'react-toastify/dist/ReactToastify.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import ru from "date-fns/locale/ru";
import moment from 'moment';
import {TitleComponent} from "../TitleComponent/TitleComponent";
import CourseScheduleService from "../../services/CourseScheduleService";
registerLocale("ru", ru);

const courseScheduleService = new CourseScheduleService();

const title = "Расписание - Student Assistant";

export class courseSchedule extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            courseScheduleModel: [],
            selectedDate: moment(new Date()).format('YYYY-MM-DD'),
            loading: true,
            groupName: 'БББО-01-16',
            counter: 0
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeCalendar = this.handleChangeCalendar.bind(this);
        this.handleChangeSelect = this.handleChangeSelect.bind(this);
        this.handleChangeNextDay = this.handleChangeNextDay.bind(this);
        this.handleChangeLastDay = this.handleChangeLastDay.bind(this);

        this.updateCourseSchedule = this.updateCourseSchedule.bind(this);

        // по дефолту отправляем Date.Now()
        this.getCourseSchedule();
    }

    //текст бокс с датой
    handleChange(event) {
        this.setState({
            selectedDate: event.target.value
        }, () => {
            this.getCourseSchedule();
        });
    }

    //следующий день
    handleChangeNextDay() {
        let myDate = new Date(this.state.selectedDate);

        myDate.setDate(myDate.getDate() + 1);

        this.setState({
            selectedDate: moment(myDate).format('YYYY-MM-DD'),
        }, () => {
            this.getCourseSchedule();
        });
    }

    //предыдущий день
    handleChangeLastDay() {
        let myDate = new Date(this.state.selectedDate);

        myDate.setDate(myDate.getDate() - 1);

        this.setState({
            selectedDate: moment(myDate).format('YYYY-MM-DD')
        }, () => {
            this.getCourseSchedule();
        });
    }

    //изменение календаря
    handleChangeCalendar(date) {
        this.setState({
            selectedDate: moment(date).format('YYYY-MM-DD')
        }, () => {
            this.getCourseSchedule();
        });
    }

    //изменение селектора групп
    handleChangeSelect(event) {
        this.setState({
            groupName: event.target.value,
        }, () => {
            this.getCourseSchedule();
        });
    }

    // обновить расписание (скачать новый файл на сервер)
    updateCourseSchedule() {
        courseScheduleService.update();
    }

    // фильтр для виджета календаря
    isWeekday(date) {
        const day = date.getDay();
        return day !== 0 //&& day !== 6 // воскресение заблокировано
    }

    // получить модель с расписанием
    getCourseSchedule() {
        courseScheduleService.get(this.state.selectedDate, this.state.groupName).then(response =>
            this.setState(
                {
                    courseScheduleModel: response.courseScheduleModel,
                    loading: response.loading
                })
        );
    }

    static renderCourseSchedule(courseScheduleModel) {

        if (courseScheduleService.validateCourseScheduleModel(courseScheduleModel)) {
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
                        <tr key={courseViewModel.id}>
                            <td>{courseViewModel.courseNumber}</td>
                            <td>{courseViewModel.startOfClasses}</td>
                            <td>{courseViewModel.endOfClasses}</td>
                            <td>
                                <div className="courseNameStyle"> {courseViewModel.courseName} </div>
                                <div className="coursePlaceStyle"> Аудитория {courseViewModel.coursePlace}</div>
                                <div className="courseTypeStyle"> {courseViewModel.courseType}</div>
                                <div className="teacherFullNameStyle"> {courseViewModel.teacherModel.fullName}</div>

                                <Popup trigger={<button className="infoIcon"></button>} position="top center">
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

        return (
            // если кол-во моделей 1 - значит есть вероятность, что модель пустая
            // далее проверяем имя первой пары и убеждаемся в этом
            <p className="infoMessage"><em>Пар не найдено. Попробуйте выбрать другой день</em></p>)
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

                    <div>
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

                        <i className="bottom">Последнее обновление
                            расписания: {this.state.courseScheduleModel.updateDatetime}.</i>

                        <img
                            src="https://image.flaticon.com/icons/svg/60/60961.svg"
                            title="Обновить расписание"
                            onClick={this.updateCourseSchedule}
                            className="iconLoad"
                            alt="iconUpdate"/>


                        <a href="https://www.mirea.ru/upload/medialibrary/0b8/KBiSP-4-kurs-1-sem.xlsx">
                            <img
                                src="https://image.flaticon.com/icons/svg/152/152555.svg"
                                title="Скачать расписание"
                                className="iconDownload"
                                alt="Download"
                            /></a>
                    </div>
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
