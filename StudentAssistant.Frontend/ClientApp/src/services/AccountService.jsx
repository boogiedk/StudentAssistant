import RestService from "./RestService";
import ToastNotificationService from "./ToastNotificationService";

const restService = new RestService();
const toastNotificationService = new ToastNotificationService();

export default class AccountService {

    login(login, password) {
        
        let requestModel = {
            login: login,
            password: password
        };

        if (!this.validateRequest(requestModel)) {
            toastNotificationService.notifyError("Некорректные данные.");
            return {
                success:false
            };
        }

        let path = '/api/v1/account/login';

        return restService.post(path, requestModel)
            .then(response => {
                if (this.validateResponse(response)) {
                    localStorage.setItem("token",response.data.token);
                    return {
                        success:true
                    };
                } else {
                    toastNotificationService.notify(response.status, "Unauthorized.");
                    return {
                        success:false
                    };
                }
            });
    }

    register(registerUser) {
        let path = '/api/v1/account/register';

        if (!this.validateRequest(registerUser)) {
            toastNotificationService.notifyError("Некорректные данные.");
            return {
                success:false
            };
        }

        return restService.post(path, registerUser)
            .then(response => {
                if (this.validateResponse(response)) {
                    localStorage.setItem("token",response.data.token);
                    return {
                        success:true
                    };
                } else {
                    toastNotificationService.notifyErrorList(response.status, response.data);
                    return {
                        success:false
                    };
                }
            });
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('token');
        return {
            success:true
        };
    }


    validateRequest(user) {
        if (user === null || typeof user === 'undefined') {
            return false;
        }

        if (user.login === null || typeof user.login === 'undefined' || user.login === '') {
            return false;
        }

        if (user.password === null || typeof user.password === 'undefined' || user.password === '') {
            return false;
        }
        
        return true; 
    }

    validateResponse(response) {
        if (response === null || typeof response === 'undefined') {
            return false;
        }

        switch (response.status) {
            case 500:
            case 404:
            case 400:
            case 401:
                return false;

            case 200:
                return true;


            default:
                return false;
        }
    }

}
