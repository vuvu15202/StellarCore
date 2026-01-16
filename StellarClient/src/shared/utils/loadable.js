import React, { lazy, Suspense } from 'react';
const loadable = (importFunc) => {
    const LazyComponent = lazy(importFunc);
    // eslint-disable-next-line react/display-name
    return props => (
        <Suspense fallback={
            <section className="content d-flex">
                <div className="sk-cube-grid">
                    <div className="sk-cube sk-cube1"></div>
                    <div className="sk-cube sk-cube2"></div>
                    <div className="sk-cube sk-cube3"></div>
                    <div className="sk-cube sk-cube4"></div>
                    <div className="sk-cube sk-cube5"></div>
                    <div className="sk-cube sk-cube6"></div>
                    <div className="sk-cube sk-cube7"></div>
                    <div className="sk-cube sk-cube8"></div>
                    <div className="sk-cube sk-cube9"></div>
                </div>  
            </section>
        }>
            <LazyComponent {...props} />
        </Suspense>
    );
};
export { loadable };