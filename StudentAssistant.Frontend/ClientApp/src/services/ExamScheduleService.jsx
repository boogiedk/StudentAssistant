import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class ExamScheduleService {

    get(groupName) {
        let path = '/api/v1/examSchedule/Get';

        let requestModel = {
            groupName: groupName
        };

        return restService.post(path, requestModel).then(response => {

            if (this.validateResponse(response)) {
                //если ок
                return {
                    examScheduleModel: response.data,
                    loading: false
                };

            } else {
                //если с ошибкой
                toastNotificationService.notify(500, "Произошла какая-то ошибка :(");
                return {
                    examScheduleModel: {},
                    loading: true
                };
            }
        });
    }

    update() {
        let path = '/api/v1/examSchedule/update';

        restService.get(path)
            .then(response => {

                if (this.validateResponse(response)) {
                    toastNotificationService.notify(response.status, response.data.message)
                } else {
                    toastNotificationService.notify(500, "Произошла какая-то ошибка :C")
                }
            });
    }

    validateExamScheduleModel(examScheduleModel) {
        if ((typeof examScheduleModel === "undefined") ||
            (examScheduleModel === null)
            || examScheduleModel.teacherModel === null)
            return false;

        return true;
    }

    validateResponse(response) {

        if ((typeof response === "undefined") ||
            (response === null))
            return false;

        return true;
    }
    

    prepareNameOfDayWeek(numberDate, dayOfWeek) {
        if (dayOfWeek === null || typeof dayOfWeek === "undefined" ||
            numberDate === null || typeof numberDate === "undefined"
        )
            return numberDate + dayOfWeek;

        // размер минимального имени дня недели, 
        // который помещается в таблицу
        if (dayOfWeek.length > 7) {
            return dayOfWeek.substring(0, 5) + ".";
        } else {
            return dayOfWeek;
        }
    }

}

