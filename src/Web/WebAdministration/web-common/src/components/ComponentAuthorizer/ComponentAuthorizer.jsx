import PropTypes from 'prop-types';

const ComponentAuthorizer = props => {
    const { Permissions, children } = props;

    const requiredPermissions = (Permissions) ? Permissions.split(',') : [];
    if (requiredPermissions.length > 0) {
        const isRestricted = true;
        if (isRestricted) {
            return null;
        }
    }
    
    return children;
};

ComponentAuthorizer.propTypes = {
    MComponent: PropTypes.element,
    Permissions: PropTypes.string
};

export default ComponentAuthorizer;
