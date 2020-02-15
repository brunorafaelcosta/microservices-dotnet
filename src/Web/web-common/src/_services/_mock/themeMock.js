import mock from '../../utils/mock';

mock.onGet('/api/theme/for/currentuser').reply(200, {
  user_id: 'bruno_costa',
  theme : {
      primary: '#1e88e5',
      secondary: '#651fff'
  }
});

