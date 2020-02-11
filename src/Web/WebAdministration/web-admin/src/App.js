import React, { Component, Suspense } from 'react';

import 'local_webcommon/src/_services/_mock';
// import './__web-common/_services/_mock';

import { ConfigContext } from './__web-common/context'
import { Bootstrap } from './__web-common/components'

import './_assets/scss/index.scss';

import config from './config'
import routes from './routes';

export default class App extends Component {
  constructor() {
    super()

    this.state = {}
  }

  render() {
    return (
      <Suspense fallback={null}>
        <ConfigContext.Provider value={config}>
          <Bootstrap config={config} routes={routes}>
          </Bootstrap>
        </ConfigContext.Provider>
      </Suspense>
    );
  }
}