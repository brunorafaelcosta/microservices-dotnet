import axios from '../utils/axios';

// import { handleResponse } from '../utils/handleResponse';

const themeService = {
    getCurrentUserTheme
};

function getCurrentUserTheme() {
    return new Promise(function (resolve, reject) {
        axios
            .get('/api/theme/for/currentuser')
            // .then(handleResponse)
            .then(theme => {
                resolve(theme)
            })
            .catch(function (error) {
                reject(error)
            })
    })
}

export default themeService;