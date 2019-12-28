﻿import {toast} from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export default class ToastNotificationService {

    notify(statusCode, message) {
        
        switch (statusCode) {
            case 200:
                toast.success(message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                });
                break;
            case 500:
                toast.error(message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                });
                break;
            case 404:
                toast.error(message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                });
                break;
                
            default:
                toast.info(message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                });
                break;
        }
    }
    
    notifyInfo(message) {
        toast.info(message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
        });
    }
}
