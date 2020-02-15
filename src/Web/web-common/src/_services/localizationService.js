import i18n from 'i18next'
import Backend from 'i18next-xhr-backend'
import { initReactI18next } from 'react-i18next'
import axios from '../utils/axios'

const localizationService = {

    defaulInitialize() {
        console.log('default localization service initialization...')
    },

    initialize(moduleName, tenantId = '00000', language = 'en', defaultLanguage = 'en') {
        this.initialized = false

        this.tenantId = tenantId
        this.moduleName = moduleName

        this.language = language
        this.defaultLanguage = defaultLanguage

        this.i18nNamespace = `${this.moduleName}.${this.tenantId}`

        let self = this
        return new Promise(function (resolve, reject) {
            return i18n
                .use(Backend)
                .use(initReactI18next)
                .init({
                    lng: self.language,
                    backend: {
                        loadPath: '/api/localization?module=' + self.moduleName + '&tenantId=' + self.tenantId + '&culture={{lng}}',
                        parse: function (data) { return data; /*return JSON.parse(data); */ },
                        ajax: (url, options, callback, data) => {
                            return axios.get(url).then((res) => {
                                callback(res.data, { status: res.status })
                            });
                        }
                    },
                    fallbackLng: self.defaultLanguage,
                    debug: true,
                    ns: [self.i18nNamespace],
                    defaultNS: self.i18nNamespace,
                    keySeparator: false,
                    interpolation: {
                        escapeValue: false,
                        formatSeparator: ','
                    },
                    react: {
                        wait: true
                    }
                }).then(d => {
                    self.initialized = true
                    resolve(true)
                });
        })
    },

    changeLanguage(language) {
        // i18n.changeLanguage(lng)
    }

};

export default localizationService