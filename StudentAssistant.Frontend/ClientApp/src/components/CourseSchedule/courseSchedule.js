import React, { Component } from 'react';
import DatePicker,{ registerLocale } from "react-datepicker";
import 'react-datepicker/dist/react-datepicker-cssmodules.css';
import ru from "date-fns/locale/ru"; 
import 'react-datepicker/dist/react-datepicker.css';

registerLocale("ru", ru);

const url = 'http://localhost:18936';

export class courseSchedule extends Component {

    constructor(props) {
        super(props);
        this.state = {
            courseScheduleModel: [],
            dateTimeString: '',
            selectedDate: new Date(),
            loading: true
        };

        this.handleChange = this.handleChange.bind(this);

        // по дефолту отправляем Date.Now()
        this.getCourseScheduleModel(this.selectedDate);
    }

    handleChange(date) {
        this.setState({
            selectedDate: date
        });

        this.getCourseScheduleModel(date);
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
                'Accept':' application/json',
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
                        <th>Тип</th>
                        <th>Кабинет</th>
                        <th>Преподаватель</th>
                        <th>Четность</th>
                        <th>Номера</th>
                    </tr>
                </thead>
                <tbody>
                    {courseScheduleModel.coursesViewModel.map(courseViewModel =>
                        <tr key={courseViewModel.courseNumber}>
                            <td>{courseViewModel.courseNumber}</td>
                            <td>{courseViewModel.courseName}</td>
                            <td>{courseViewModel.courseType}</td>
                            <td>{courseViewModel.coursePlace}</td>
                            <td>{courseViewModel.teacherFullName}</td>
                            <td>{courseViewModel.parityWeek}</td>
                            <td>{courseViewModel.numberWeek}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    componentDidMount() {
        var that = this;
        var date = new Date().getDate(); //Current Date
        var month = new Date().getMonth() + 1; //Current Month
        var year = new Date().getFullYear(); //Current Year
        var hours = new Date().getHours(); //Current Hours
        var min = new Date().getMinutes(); //Current Minutes
        //  var sec = new Date().getSeconds(); //Current Seconds

        that.setState({
            //Setting the value of the date time
            dateTimeString:
                date + '.' + month + '.' + year + ' ' + hours + ':' + min, // + ':' + sec,
        });

       


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

                <p>На странице отображено расписание на <b>{this.state.courseScheduleModel.nameOfDayWeek}</b></p> 
                
                <DatePicker
                    selected={this.state.selectedDate}
                    onChange={this.handleChange}
                    dateFormat="dd.MM.yyyy"
                    locale = "ru"            
                    />

                {contents}

            </div>
        );
    }
}
