/* eslint-disable no-undef */
import React from 'react'
import { Helmet } from 'react-helmet'
import { withRouter } from 'react-router'
import PropTypes from 'prop-types'
import { compose } from 'recompose'

const NODE_ENV = process.env.NODE_ENV;
const GA_MEASUREMENT_ID = process.env.REACT_APP_GA_MEASUREMENT_ID;

class Page extends React.Component {

  static propTypes = {
    match: PropTypes.object.isRequired,
    location: PropTypes.object.isRequired,
    history: PropTypes.object.isRequired,

    children: PropTypes.node,
    title: PropTypes.string
  }

  constructor(props) {
    super(props)
  }

  componentDidMount() {
    // useEffect(() => {
    //   if (NODE_ENV !== 'production') {
    //     return;
    //   }

    //   if (window.gtag) {
    //     window.gtag('config', GA_MEASUREMENT_ID, {
    //       page_path: routerContext.location.pathname,
    //       page_name: title
    //     });
    //   }
    // }, [title, routerContext]);
  }

  render() {
    const { match, location, history, title, children, staticContext, ...rest } = this.props
    const router = { match, location, history }

    return (
      <div {...rest}>
        <Helmet>
          <title>{title}</title>
        </Helmet>
        {children}
      </div>
    )
  }
}

export default compose(
  withRouter
)(Page)
