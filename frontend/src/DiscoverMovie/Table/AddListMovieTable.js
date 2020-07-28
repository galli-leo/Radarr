import PropTypes from 'prop-types';
import React, { Component } from 'react';
import VirtualTable from 'Components/Table/VirtualTable';
import VirtualTableRow from 'Components/Table/VirtualTableRow';
import AddListMovieItemConnector from 'DiscoverMovie/AddListMovieItemConnector';
import { sortDirections } from 'Helpers/Props';
import getIndexOfFirstCharacter from 'Utilities/Array/getIndexOfFirstCharacter';
import AddListMovieHeaderConnector from './AddListMovieHeaderConnector';
import AddListMovieRowConnector from './AddListMovieRowConnector';
import styles from './AddListMovieTable.css';

class AddListMovieTable extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      scrollIndex: null
    };
  }

  componentDidUpdate(prevProps) {
    const {
      items,
      jumpToCharacter
    } = this.props;

    if (jumpToCharacter != null && jumpToCharacter !== prevProps.jumpToCharacter) {

      const scrollIndex = getIndexOfFirstCharacter(items, jumpToCharacter);

      if (scrollIndex != null) {
        this.setState({ scrollIndex });
      }
    } else if (jumpToCharacter == null && prevProps.jumpToCharacter != null) {
      this.setState({ scrollIndex: null });
    }
  }

  //
  // Control

  rowRenderer = ({ key, rowIndex, style }) => {
    const {
      items,
      columns,
      selectedState,
      onSelectedChange
    } = this.props;

    const movie = items[rowIndex];

    return (
      <VirtualTableRow
        key={key}
        style={style}
      >
        <AddListMovieItemConnector
          key={movie.tmdbId}
          component={AddListMovieRowConnector}
          columns={columns}
          movieId={movie.tmdbId}
          isSelected={selectedState[movie.tmdbId]}
          onSelectedChange={onSelectedChange}
        />
      </VirtualTableRow>
    );
  }

  //
  // Render

  render() {
    const {
      items,
      columns,
      sortKey,
      sortDirection,
      isSmallScreen,
      onSortPress,
      scroller,
      allSelected,
      allUnselected,
      onSelectAllChange,
      selectedState
    } = this.props;

    return (
      <VirtualTable
        className={styles.tableContainer}
        items={items}
        scrollIndex={this.state.scrollIndex}
        isSmallScreen={isSmallScreen}
        scroller={scroller}
        rowHeight={38}
        overscanRowCount={2}
        rowRenderer={this.rowRenderer}
        header={
          <AddListMovieHeaderConnector
            columns={columns}
            sortKey={sortKey}
            sortDirection={sortDirection}
            onSortPress={onSortPress}
            allSelected={allSelected}
            allUnselected={allUnselected}
            onSelectAllChange={onSelectAllChange}
          />
        }
        selectedState={selectedState}
        columns={columns}
      />
    );
  }
}

AddListMovieTable.propTypes = {
  items: PropTypes.arrayOf(PropTypes.object).isRequired,
  columns: PropTypes.arrayOf(PropTypes.object).isRequired,
  sortKey: PropTypes.string,
  sortDirection: PropTypes.oneOf(sortDirections.all),
  jumpToCharacter: PropTypes.string,
  isSmallScreen: PropTypes.bool.isRequired,
  scroller: PropTypes.instanceOf(Element).isRequired,
  onSortPress: PropTypes.func.isRequired,
  allSelected: PropTypes.bool.isRequired,
  allUnselected: PropTypes.bool.isRequired,
  selectedState: PropTypes.object.isRequired,
  onSelectedChange: PropTypes.func.isRequired,
  onSelectAllChange: PropTypes.func.isRequired
};

export default AddListMovieTable;
