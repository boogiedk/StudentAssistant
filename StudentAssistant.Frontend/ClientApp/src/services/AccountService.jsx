import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class AccountService {

    login(login, password) {

        let requestModel = {
            Login: login,
            Password: password
        };

        if (!this.validate(requestModel)) {
            return "error";
        }

        let path = '/api/v1/account/login';

        let token = null;

        return restService.post(path, requestModel).then(response => {

            if (this.validate(response)) {
                //если ок
                return {
                    token: response.data
                };

            } else {
                //если с ошибкой
                toastNotificationService.notify(500, "Произошла какая-то ошибка :(");
                return {
                    token: {},
                };
            }
        });


    }

    register(login, password) {

        let requestModel = {
            Login: login,
            Password: password
        };

        if (!this.validate(requestModel)) {
            return "error";
        }

        let path = '/api/v1/account/register';

        return restService.post(path, requestModel).then(response => {

            if (this.validate(response)) {
                //если ок
                return {
                    token: response.data
                };

            } else {
                //если с ошибкой
                toastNotificationService.notify(500, "Произошла какая-то ошибка :(");
                return {
                    token: {},
                };
            }
        });


    }


    validate(user) {
        if (user === null || typeof user === 'undefined') {
            return false;
        }

        if (user.Login === null || typeof user.Login === 'undefined') {
            return false;
        }

        if (user.Password === null || typeof user.Password === 'undefined') {
            return false;
        }

        return true;
    }
}
