import { makeStyles } from '@material-ui/core/styles';

export const useStyles = makeStyles(theme => ({
  root: {
    width: '100%',
    '& > * + *': {
      marginTop: theme.spacing(2)
    }
  }
}));

export const emailRegex = /^\w+([.-]?\w+)+@\w+([.:]?\w+)+(\.[a-zA-Z0-9]{2,3})+$/;

export const columns = [
  {
    title: 'Id Number',
    field: 'IdNumber',
    type: 'numeric',
    editable: 'onAdd'
  },
  { title: 'Full Name', field: 'FullName', type: 'string' },
  { title: 'Email', field: 'Email', type: 'string' },
  {
    title: 'Birth Date',
    field: 'BirthDate',
    type: 'date'
  },
  {
    title: 'Gender',
    field: 'Gender',
    lookup: { '': '', Male: 'Male', Female: 'Female', Other: 'Other' }
  },
  {
    title: 'Phone Number',
    field: 'PhoneNum',
    type: 'numeric'
  }
];
