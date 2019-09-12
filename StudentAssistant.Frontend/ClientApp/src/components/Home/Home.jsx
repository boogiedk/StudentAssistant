import React, {Component} from 'react';
import {TitleComponent} from "../TitleComponent/TitleComponent";

const title = "Главная - Student Assistant";

export class Home extends Component {
    static displayName = Home.name;

    /*костыль*/
    static getTitle() {
        return (
            <React.Fragment>
                <TitleComponent title={title}/>
            </React.Fragment>
        );
    }

    render() {
        return (
            <div>
                {Home.getTitle()} 
                <h1>Добро пожаловать!</h1>
                <p>Добро пожаловать на сервис, который умеет: </p>
                <ul>
                    <li>Отображать текущее учебное расписание</li>
                    <li>Расчитывать номер и четность недели</li>
                    <li>И еще какая-нибудь интересная строчка</li>
                </ul>
                <p>Как вы можете сделать сервис лучше: </p>
                <ul>
                    <li><a href="tg://resolve?domain=studyAss" rel="noopener">Писать баг-репорты разработчику</a> для
                        оперативного исправления ошибок
                    </li>
                    <li><strong>Давать регулярный feedback</strong>, чтобы мы могли создавать то, чем Вы действительно
                        будете пользоваться с удовольствием!
                    </li>
                    <li>И, наконец, <strong>самим стать разработчиком</strong>. Будь-то вклад как в <a
                        href='https://github.com/boogiedk/StudentAssistant' rel="noopener">opensourse</a> проект или же
                        разработка в команде <strong>StudyAssTeam</strong> - любая помощь поможет стать нам лучше!
                    </li>
                </ul>
                <p>У проекта есть свой <a href='https://trello.com/b/TXtoDDO0/student-assistant-kanban'
                                          rel="noopener">Roadmap</a> разработки в Trello, чтобы пользователи видели, над
                    чем ведется работа, а над чем она закончена.
                    Так же там публикаются все новинки и релизы.
                    Не пропустите!</p>
                <p><strong>Спасибо!</strong></p>
            </div>
        );
    }
}
