import mock from '../../utils/mock';

mock.onGet('/api/localization?module=web-admin&tenantId=00000&culture=en').reply(200, {
    "hello.label": "Hello",
    "thankyou.label": "Thank you",

    "signout.label": "Sing out",

    "menutitle.pages": "Pages",
    "menutitle.dashboards": "Dashboards",
    "menutitle.dashboards.default": "Default"
});

