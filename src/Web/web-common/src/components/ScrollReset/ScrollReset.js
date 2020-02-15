import React from 'react';
import { withRouter } from 'react-router'

class ScrollReset extends React.Component {
  constructor(props) {
    super(props)
  }

  componentDidMount() {
    window.scrollTo(0, 0);
  }
  
  render() {
    return null;
  }
}

export default withRouter(ScrollReset);
