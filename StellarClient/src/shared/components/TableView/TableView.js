import React, {useState, useEffect, useMemo} from 'react';
import PropTypes from 'prop-types';
import {Spin, Input, Table, Select, Pagination, Row, Col, Button, Space} from 'antd';
import {Icon} from '../Icon';

const SortType = {
    '1': 'ascend',
    '-1': 'descend'
};

const UpdateColumnsSort = (columns, sortOption = {}) => {
    return columns.map(col => {
        let newCol = {...col};
        if (newCol.children) {
            newCol.children = UpdateColumnsSort(newCol.children, sortOption);
        }
        newCol.sortOrder = SortType[sortOption[newCol.key]];
        return newCol;
    });

};

export const TableView = (props) => {
    const {
        loading,
        className,
        hasSearch,
        AdvanceFilter,
        defaultAdvSearchOpen,
        ActionBar,
        search,
        columns,
        handleChange,
        expandable,
        pagination = true,
        searchSpan,
        metadata,
        ...rest
    } = props;

    const [isAdvSearchOpen, setIsAdvSearchOpen] = useState(defaultAdvSearchOpen);
    const [keySearch, setKeySearch] = useState(search);
    const [pageSize, setPageSize] = useState(metadata?.page_size || 20); // Dùng giá trị từ metadata
    const [totalRecord, setTotalRecord] = useState(metadata?.total || 0); // Dùng giá trị từ metadata
    const [currentPage, setCurrentPage] = useState(metadata?.page || 1);
    const [sortOption, setSortOption] = useState(metadata?.sort);

    const cols = useMemo(() => {
        return UpdateColumnsSort(columns, sortOption);
    }, [columns, sortOption]);

    useEffect(() => {
        if (metadata) {
            setTotalRecord(metadata.total);
            setCurrentPage(metadata.page);
            setPageSize(metadata.page_size);
        }
    }, [metadata]);

    useEffect(() => {
        if (handleChange) {
            handleChange({
                search: keySearch,
                page_size: pageSize,
                page: currentPage,
                sort: sortOption
            });
        }
    }, [keySearch, pageSize, sortOption]);

    const onChangePage = (page) => {
        setCurrentPage(page);
        if (handleChange) {
            handleChange({
                search: keySearch,
                page_size: pageSize,
                page,
                sort: sortOption
            });
        }
    };

    const handleSearch = (value) => {
        setKeySearch(value);
        setCurrentPage(1);
        if (handleChange) {
            handleChange({
                search: value,
                page: 1,
                page_size: pageSize,
                sort: sortOption
            });
        }
    };

    const onChangepageSize = (size) => {
        setCurrentPage(1);
        setPageSize(size);
        if (handleChange) {
            handleChange({
                search: keySearch,
                page: 1,
                page_size: size,
                sort: sortOption
            });
        }
    };
    const handleTableChange = (pagination, filters, sorter) => {
        if (pagination.current !== currentPage) {
            setCurrentPage(pagination.current ?? 1);
        }
        if (sorter) {
            if (Array.isArray(sorter)) {
                const sortOtp = {};
                sorter.forEach(item => {
                    sortOtp[item.columnKey] = item.order === 'ascend' ? 1 : -1;
                });
                setSortOption(sortOtp);
            } else {
                const sortOtp = {};
                if (sorter.order) {
                    sortOtp[sorter.columnKey] = sorter.order === 'ascend' ? 1 : -1;
                }
                setSortOption(sortOtp);
            }
        }
    };
    const onChangeSearch = (params) => {
        setKeySearch(params?.target?.value);
    };
    return (
        <Spin spinning={loading}>
            <Row gutter={[16, 16]} className={['grid-view', className].join(' ')}>
                <Col span={24} className='grid-header'>
                    <Row gutter={[16, 16]}>

                        <Col xs={{span: 24, order: 2}} md={{span: searchSpan ?? 10, order: 1}}>
                            <Row>
                                {hasSearch && <Col span={12}>
                                    <Input.Search
                                        // addonAfter={}
                                        className='search'
                                        defaultValue={keySearch}
                                        onSearch={handleSearch}
                                        onBlur={onChangeSearch}
                                        placeholder="Tìm kiếm nhanh"
                                        allowClear/>
                                </Col>}

                                <Col span={12}>
                                    {AdvanceFilter ?
                                        <Button className='btn-adv-search' type="link"
                                            onClick={() => setIsAdvSearchOpen(!isAdvSearchOpen)}>
                                            <Space>Tìm kiếm nâng cao<Icon
                                                icon={isAdvSearchOpen ? 'ant-design:down-outlined' : 'ant-design:up-outlined'}/></Space>
                                        </Button> : null}
                                </Col>
                            </Row>


                        </Col>
                        {ActionBar &&
                            <Col xs={{span: 24, order: 1}}
                                md={{
                                    span: (hasSearch || !!AdvanceFilter) ? (searchSpan ? 24 - searchSpan : 14) : 24,
                                    order: 2
                                }}
                                className="grid-view-action-bar">
                                <div className="float-sm-end">
                                    {ActionBar}
                                </div>
                            </Col>
                        }
                        {isAdvSearchOpen &&
                            <Col span={24} order={3} className="filter-adv-container">
                                <fieldset>
                                    <legend>Tham số tìm kiếm</legend>
                                    {AdvanceFilter}
                                </fieldset>
                            </Col>
                        }
                    </Row>
                </Col>

                <Col span={24} className='grid-table'>
                    <Table
                        {...rest}
                        columns={cols}
                        onChange={handleTableChange}
                        pagination={false}
                        size='small'
                        expandable={expandable}
                    >

                    </Table>

                </Col>
                {pagination &&
                    <Col span={24} className="grid-pagination">
                        <div className="d-none d-sm-flex">
                            {!props.hidePageSizeSelect && pageSize > 0 &&
                                <div className="select-page-size">
                                    Hiển thị <Select
                                        size='small'
                                        className='change-page-size'
                                        value={pageSize}
                                        onChange={onChangepageSize}
                                        showSearch
                                        options={[
                                            {value: 10, label: '10 / trang'},
                                            {value: 20, label: '20 / trang'},
                                            {value: 50, label: '50 / trang'},
                                            {value: 100, label: '100 / trang'},
                                        ]}
                                    />
                                </div>
                            }
                        </div>
                        <Pagination
                            className='float-end justify-content-center'
                            total={totalRecord}
                            pageSize={pageSize > 0 ? pageSize : totalRecord}
                            current={currentPage}
                            onChange={onChangePage}
                            simple
                            showTotal={(total, ranger) => {
                                return (<span> {ranger[0]} - {ranger[1]} trong số {total}</span>);
                            }}/>
                    </Col>
                }
            </Row>

        </Spin>
    );
};
TableView.propTypes = {
    loading: PropTypes.bool,
    hasSearch: PropTypes.bool,
    AdvanceFilter: PropTypes.element,
    ActionBar: PropTypes.element,
    handleChange: PropTypes.func,
    className: PropTypes.string,
    page: PropTypes.number,
    page_size: PropTypes.number,
    total: PropTypes.number,
    search: PropTypes.string,
    sort: PropTypes.object,
    defaultFilter: PropTypes.object,
    columns: PropTypes.array,
    defaultAdvSearchOpen: PropTypes.bool,
    pagination: PropTypes.bool,
    metadata: PropTypes.object,
    expandable: PropTypes.object,
};