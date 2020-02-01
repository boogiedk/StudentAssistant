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

        if (!this.validateRequest(requestModel)) {
            toastNotificationService.notify(400, "Некорректные данные.");
        }

        let path = '/api/v1/account/login';

        return restService.post(path, requestModel).then(response => {
            if (this.validateResponse(response)) {
                return {
                    token: response.data
                };
            } else {
                toastNotificationService.notify(400, "Что-то пошло не так :C");
            }
            console.log(response)
        });
    }

    register(registerUser) {
        console.log(registerUser);

        let path = '/api/v1/account/register';

        return restService.post(path, registerUser).then(function (response) {
           toastNotificationService.notifyErrorList(404,response.data);
           // console.log(response.data)
        });
    }


    validateRequest(user) {
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

    validateResponse(response) {
        if (response === null || typeof response === 'undefined') {
            return false;
        }

        if (response.status === 401) {
            return false;
        }

        if (response.status === 400) {
            return false;
        }

        if (response.status === 500) {
            return false;
        }


        return true;
    }

}
