import PropTypes from 'prop-types';
import React, { Component } from 'react';
import LoadingIndicator from 'Components/Loading/LoadingIndicator';
import Table from 'Components/Table/Table';
import TableBody from 'Components/Table/TableBody';
// import MovieHistoryRowConnector from './MovieHistoryRowConnector';
const columns = [
  {
    name: 'eventType',
    isVisible: true
  },
  {
    name: 'sourceTitle',
    label: 'Source Title',
    isVisible: true
  },
  {
    name: 'quality',
    label: 'Quality',
    isVisible: true
  },
  {
    name: 'date',
    label: 'Date',
    isVisible: true
  },
  {
    name: 'details',
    label: 'Details',
    isVisible: true
  },
  {
    name: 'actions',
    label: 'Actions',
    isVisible: true
  }
];

class MovieTitlesTableContent extends Component {

  //
  // Render

  render() {
    const {
      isFetching,
      isPopulated,
      error,
      items
    } = this.props;

    const hasItems = !!items.length;
    return (
      <div>
        {
          isFetching &&
            <LoadingIndicator />
        }

        {
          !isFetching && !!error &&
            <div>Unable to load history.</div>
        }

        {
          isPopulated && !hasItems && !error &&
            <div>No history.</div>
        }

        {
          isPopulated && hasItems && !error &&
          <Table columns={columns}>
            <TableBody>
              {
              }
            </TableBody>
          </Table>
        }
      </div>
    );
  }
}

MovieTitlesTableContent.propTypes = {
  isFetching: PropTypes.bool.isRequired,
  isPopulated: PropTypes.bool.isRequired,
  error: PropTypes.object,
  items: PropTypes.arrayOf(PropTypes.object).isRequired
};

export default MovieTitlesTableContent;