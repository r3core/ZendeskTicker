import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class Layout extends Component {
  displayName = Layout.name

  render() {
    return (
      <Grid alignItems="center">
        <Row>
          <Col>
            {this.props.children}
          </Col>
        </Row>
      </Grid>
    );
  }
}
