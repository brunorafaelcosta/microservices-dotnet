const moduleNavigation = [
    {
        title: 'menutitle.pages',
        permissions: '',
        pages: [
            {
                title: 'menutitle.dashboards',
                href: '/dashboards',
                permissions: '',
                children: [
                    {
                        title: 'menutitle.dashboards.default',
                        href: '/dashboards/default'
                    }
                ]
            }
        ]
    }
];

export default moduleNavigation;