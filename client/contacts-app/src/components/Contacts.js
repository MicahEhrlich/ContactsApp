import React, { useEffect } from 'react';
import MaterialTable from 'material-table';
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';
import { useStyles, columns, emailRegex } from '../constants';
import axios from 'axios';

function Alert(props) {
  return <MuiAlert elevation={6} variant='filled' {...props} />;
}

const CONTACTS_API_URL = 'https://localhost:44339/api/contact';

const Contacts = () => {
  const classes = useStyles();
  const [open, setOpen] = React.useState(false);
  const [alertMsg, setAlertMsg] = React.useState('');
  const [severity, setSeverity] = React.useState('');
  const [state, setState] = React.useState({});

  useEffect(() => {
    getContacts();
  }, []);

  const getContacts = async () => {
    const res = await axios.get(`${CONTACTS_API_URL}`);
    setState(res);
  };

  const addContact = async contact => {
    await axios
      .post(`${CONTACTS_API_URL}`, contact, {
        headers: { 'Content-Type': 'application/json' }
      })
      .then(
        response => {
          getContacts();
          showAlert('success', 'Contact added successfully');
        },
        error => {
          showAlert('error', 'Failed to add contact');
        }
      );
  };

  const updateContact = async contact => {
    await axios
      .put(`${CONTACTS_API_URL}`, contact, {
        headers: { 'Content-Type': 'application/json' }
      })
      .then(
        response => {
          getContacts();
          showAlert('success', 'Contact updated successfully');
        },
        error => {
          showAlert('error', 'Failed to update contact');
        }
      );
  };

  const deleteContact = async idNumber => {
    await axios.delete(`${CONTACTS_API_URL}/${idNumber}`).then(
      response => {
        getContacts();
        showAlert('success', 'Contact deleted successfully');
      },
      error => {
        showAlert('error', 'Failed to delete contact');
      }
    );
  };

  const handleClose = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }

    setOpen(false);
  };

  const showAlert = (severity, message) => {
    setSeverity(severity);
    setAlertMsg(message);
    setOpen(true);
  };

  const validateFields = newData => {
    if (!('IdNumber' in newData) || newData.IdNumber.length === 0) {
      setSeverity('error');
      setAlertMsg('Id Number is required');
      setOpen(true);
      return false;
    }
    if (!('FullName' in newData) || newData.FullName.length === 0) {
      setSeverity('error');
      setAlertMsg('Full Name is required');
      setOpen(true);
      return false;
    }
    if (
      !('BirthDate' in newData) ||
      newData.BirthDate.length === 0 ||
      newData.BirthDate > new Date()
    ) {
      setSeverity('error');
      setAlertMsg('Birth Date is required');
      setOpen(true);
      return false;
    }
    if ('Email' in newData && newData.Email.length > 0) {
      if (!emailRegex.test(newData.Email.toLowerCase())) {
        setSeverity('error');
        setAlertMsg('Invalid Email Address');
        setOpen(true);
        return false;
      }
    }
    return true;
  };

  return (
    <div>
      <MaterialTable
        title='Contacts'
        columns={columns}
        data={state.data}
        editable={{
          onRowAdd: newData =>
            new Promise((resolve, reject) => {
              setTimeout(() => {
                if (validateFields(newData)) {
                  addContact(newData);
                  resolve();
                } else {
                  reject();
                }
              }, 600);
            }),
          onRowUpdate: newData =>
            new Promise((resolve, reject) => {
              setTimeout(() => {
                if (validateFields(newData)) {
                  resolve();
                  updateContact(newData);
                } else {
                  reject();
                }
              }, 600);
            }),
          onRowDelete: oldData =>
            new Promise(resolve => {
              setTimeout(() => {
                resolve();
                deleteContact(oldData.IdNumber);
              }, 600);
            })
        }}
      />
      <div className={classes.root}>
        <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
          <Alert onClose={handleClose} severity={severity}>
            {alertMsg}
          </Alert>
        </Snackbar>
      </div>
    </div>
  );
};

export default Contacts;
