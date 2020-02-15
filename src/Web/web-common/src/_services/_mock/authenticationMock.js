import uuid from 'uuid/v1';

import mock from '../../utils/mock';

mock.onGet('/api/getauthenticateduser').reply(200, {
  user: {
    name: 'Bruno Rafael Costa',
    tenantId: '00000',
    culture: 'en'
  }
});

