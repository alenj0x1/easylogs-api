/*
  easylogs Database
*/

create table Permission (
  permission_id serial primary key not null,
  name varchar(50) not null,
  show_name varchar(50) not null,
  description varchar(255) default ('without description') not null,
  created_at timestamptz default(now()) not null,
  updated_at timestamptz default(now()) not null
);

insert into Permission (name, show_name, description)
values
('ADMINISTRATOR',       'Administrator',       'All permissions in the application'),
('CREATE_LOGS',         'Create logs',         'Allowed for logs creation'),
('DELETE_LOGS',         'Delete logs',         'Allowed for logs deletion'),
('CREATE_USERS',        'Create users',        'Allowed for create users'),
('UPDATE_USERS',        'Update users',        'Allowed for update users'),
('DELETE_USERS',        'Delete users',        'Allowed for remove users'),
('CREATE_ACCESS_TOKEN', 'Create access token', 'Allowed for create access tokens'),
('REMOVE_ACCESS_TOKEN', 'Remove access token', 'Allowed for remove access tokens');

create table SessionType (
  session_type_id serial primary key not null,
  name varchar(100) not null,
  created_at timestamptz default(now()) not null,
  updated_at timestamptz default(now()) not null
);

insert into SessionType (name)
values 
('infinity'), 
('period');

create table UserApp (
  user_app_id uuid primary key default(gen_random_uuid()),
  username varchar(100) not null,
  email varchar(255) not null,
  password varchar(255) not null,
  session_type_id int references SessionType(session_type_id) default(1) not null,
  session_time int default(1) not null,
  created_at timestamptz default(now()) not null,
  updated_at timestamptz default(now()) not null,
  deleted_at timestamptz
);

create table UserAppPermission (
  user_app_permission_id serial primary key not null,
  user_app_id uuid references UserApp(user_app_id) not null,
  permission_id int references Permission(permission_id) not null,
  given_at timestamptz default(now()) not null
);

create table LogType (
  log_type_id serial primary key not null,
  name varchar(100) not null,
  show_name varchar(100) not null,
  style_class varchar(255) not null,
  created_at timestamptz default(now()) not null,
  updated_at timestamptz default(now()) not null
);

insert into LogType (name, show_name, style_class)
values
('debug', 'Debug', 'bg-purple-400 text-purple-100 border-purple-300 border-2 px-3 py-1 rounded-full text-sm'), 
('information', 'Información', 'bg-teal-400 text-teal-100 border-teal-300 border-2 px-3 py-1 rounded-full text-sm'), 
('success', 'Correcto', 'bg-green-400 text-green-100 border-green-300 border-2 px-3 py-1 rounded-full text-sm'), 
('warning', 'Precaución', 'bg-amber-400 text-amber-100 border-amber-300 border-2 px-3 py-1 rounded-full text-sm'), 
('error', 'Error', 'bg-red-400 text-red-100 border-red-300 border-2 px-3 py-1 rounded-full text-sm');

create table Log (
  log_id uuid primary key default(gen_random_uuid()),
  message text not null,
  trace text default('empty') not null,
  exception text,
  stack_trace text,
  type int references LogType(log_type_id) default(2) not null,
  data_json text default('{}') not null,
  created_at timestamptz default(now()) not null
);

create table TokenRefresh (
	token_refresh_id serial primary key not null,
	user_app_id uuid references UserApp(user_app_id) not null,
  ip varchar(255) not null,
  is_api_key boolean default(false) not null,
	value text not null,
	created_at timestamptz not null default(now()),
	expiration timestamptz
);

CREATE TABLE TokenAccess (
	token_access_id serial primary key not null,
	token_refresh_id int not null references TokenRefresh(token_refresh_id) on delete cascade,
	user_app_id uuid references UserApp(user_app_id) not null,
  ip varchar(255) not null,
  is_api_key boolean default(false) not null,
	value text not null,
	created_at timestamptz not null default(now()),
	expiration timestamptz not null
);
