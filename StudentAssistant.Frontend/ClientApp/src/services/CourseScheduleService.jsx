import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class CourseScheduleService {

    get(selectedDatetime, groupName) {
        let path = '/api/v1/schedule/selected';

        let requestModel = {
            dateTimeRequest: selectedDatetime,
            groupName: groupName
        };

      return restService.post(path, requestModel).then(response => {

            if (this.validateResponse(response)) {
                //если ок
                return {
                    courseScheduleModel: response.data,
                    loading: false
                };

            } else {
                //если с ошибкой
                toastNotificationService.notify(500, "Произошла какая-то ошибка :(");
                return {
                    courseScheduleModel: {},
                    loading: true
                };
            }
        });
    }

    update() {
        let path = '/api/v1/schedule/update';

        restService.get(path)
            .then(response => {

                if (this.validateResponse(response)) {
                    toastNotificationService.notify(response.status, response.data.message)
                } else {
                    toastNotificationService.notify(500, "Произошла какая-то ошибка :C")
                }
            });
    }

    validateCourseScheduleModel(courseScheduleModel) {
        if ((typeof courseScheduleModel === "undefined") ||
            (courseScheduleModel === null) || (
                courseScheduleModel.coursesViewModel.length === 1 &
                courseScheduleModel.coursesViewModel[0].courseName === ""))
            return false;

        return true;
    }

    validateResponse(response) {

        if ((typeof response === "undefined") ||
            (response === null))
            return false;

        return true;
    }
}

